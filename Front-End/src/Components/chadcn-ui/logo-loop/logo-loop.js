import { useCallback, useEffect, useMemo, useRef, useState, memo } from "react";

const ANIMATION_CONFIG = {
    SMOOTH_TAU: 0.25,
    MIN_COPIES: 2,
    COPY_HEADROOM: 2,
};

const toCssLength = (value) =>
    typeof value === "number" ? `${value}px` : value ?? undefined;

const cx = (...parts) => parts.filter(Boolean).join(" ");

const useResizeObserver = (callback, elements, dependencies) => {
    useEffect(() => {
        if (!window.ResizeObserver) {
            const handleResize = () => callback();
            window.addEventListener("resize", handleResize);
            callback();
            return () => window.removeEventListener("resize", handleResize);
        }

        const observers = elements.map((ref) => {
            if (!ref.current) return null;
            const observer = new ResizeObserver(callback);
            observer.observe(ref.current);
            return observer;
        });

        callback();

        return () => {
            observers.forEach((observer) => observer?.disconnect());
        };
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, dependencies);
};

const useImageLoader = (seqRef, onLoad, dependencies) => {
    useEffect(() => {
        const images = seqRef.current?.querySelectorAll("img") ?? [];

        if (images.length === 0) {
            onLoad();
            return;
        }

        let remainingImages = images.length;
        const handleImageLoad = () => {
            remainingImages -= 1;
            if (remainingImages === 0) {
                onLoad();
            }
        };

        images.forEach((img) => {
            if (img.complete) {
                handleImageLoad();
            } else {
                img.addEventListener("load", handleImageLoad, { once: true });
                img.addEventListener("error", handleImageLoad, { once: true });
            }
        });

        return () => {
            images.forEach((img) => {
                img.removeEventListener("load", handleImageLoad);
                img.removeEventListener("error", handleImageLoad);
            });
        };
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, dependencies);
};

const useAnimationLoop = (
    trackRef,
    targetVelocity,
    seqWidth,
    isHovered,
    pauseOnHover,
    direction
) => {
    const rafRef = useRef(null);
    const lastTimestampRef = useRef(null);
    const offsetRef = useRef(0);
    const velocityRef = useRef(0);

    useEffect(() => {
        const track = trackRef.current;
        if (!track) return;

        const animate = (timestamp) => {
            if (lastTimestampRef.current === null) {
                lastTimestampRef.current = timestamp;
            }

            const deltaTime = Math.max(
                0,
                timestamp - lastTimestampRef.current
            ) / 1000;
            lastTimestampRef.current = timestamp;

            const target = pauseOnHover && isHovered ? 0 : targetVelocity;

            const easingFactor = 1 - Math.exp(-deltaTime / ANIMATION_CONFIG.SMOOTH_TAU);
            velocityRef.current += (target - velocityRef.current) * easingFactor;

            if (seqWidth > 0) {
                let nextOffset = offsetRef.current + velocityRef.current * deltaTime;
                nextOffset = ((nextOffset % seqWidth) + seqWidth) % seqWidth;
                offsetRef.current = nextOffset;

                const translateX =
                    direction === "left" ? -offsetRef.current : offsetRef.current;

                track.style.transform = `translate3d(${translateX}px, 0, 0)`;
            }

            rafRef.current = requestAnimationFrame(animate);
        };

        rafRef.current = requestAnimationFrame(animate);
        return () => {
            if (rafRef.current) cancelAnimationFrame(rafRef.current);
            lastTimestampRef.current = null;
        };
    }, [targetVelocity, seqWidth, isHovered, pauseOnHover, trackRef, direction]);
};

export const LogoLoop = memo(
    ({
        logos,
        speed = 120,
        direction = "left", // left للإنجليزي، right للعربي
        width = "100%",
        logoHeight = 28,
        gap = 32,
        pauseOnHover = true,
        fadeOut = false,
        fadeOutColor,
        scaleOnHover = false,
        ariaLabel = "Partner logos",
        className,
        style,
    }) => {
        const containerRef = useRef(null);
        const trackRef = useRef(null);
        const seqRef = useRef(null);

        const [seqWidth, setSeqWidth] = useState(0);
        const [copyCount, setCopyCount] = useState(ANIMATION_CONFIG.MIN_COPIES);
        const [isHovered, setIsHovered] = useState(false);

        const targetVelocity = useMemo(() => {
            const magnitude = Math.abs(speed);
            const directionMultiplier = direction === "left" ? 1 : -1;
            const speedMultiplier = speed < 0 ? -1 : 1;
            return magnitude * directionMultiplier * speedMultiplier;
        }, [speed, direction]);

        const updateDimensions = useCallback(() => {
            const containerWidth = containerRef.current?.clientWidth ?? 0;
            const sequenceWidth = seqRef.current?.getBoundingClientRect?.()?.width ?? 0;

            if (sequenceWidth > 0) {
                setSeqWidth(Math.ceil(sequenceWidth));
                const copiesNeeded =
                    Math.ceil(containerWidth / sequenceWidth) + ANIMATION_CONFIG.COPY_HEADROOM;
                setCopyCount(Math.max(ANIMATION_CONFIG.MIN_COPIES, copiesNeeded));
            }
        }, []);

        useResizeObserver(updateDimensions, [containerRef, seqRef], [logos, gap, logoHeight]);
        useImageLoader(seqRef, updateDimensions, [logos, gap, logoHeight]);

        useAnimationLoop(trackRef, targetVelocity, seqWidth, isHovered, pauseOnHover, direction);

        const cssVariables = useMemo(
            () => ({
                "--logoloop-gap": `${gap}px`,
                "--logoloop-logoHeight": `${logoHeight}px`,
                ...(fadeOutColor && { "--logoloop-fadeColor": fadeOutColor }),
            }),
            [gap, logoHeight, fadeOutColor]
        );

        const rootClasses = useMemo(
            () =>
                cx(
                    "relative overflow-hidden group",
                    "[--logoloop-gap:32px]",
                    "[--logoloop-logoHeight:28px]",
                    "[--logoloop-fadeColorAuto:#ffffff]",
                    "dark:[--logoloop-fadeColorAuto:#0b0b0b]",
                    scaleOnHover && "py-[calc(var(--logoloop-logoHeight)*0.1)]",
                    className
                ),
            [scaleOnHover, className]
        );

        const handleMouseEnter = useCallback(() => {
            if (pauseOnHover) setIsHovered(true);
        }, [pauseOnHover]);

        const handleMouseLeave = useCallback(() => {
            if (pauseOnHover) setIsHovered(false);
        }, [pauseOnHover]);

        const renderLogoItem = useCallback(
            (item, key) => (
                <li
                    className={cx(
                        "flex-none mr-[var(--logoloop-gap)] text-[length:var(--logoloop-logoHeight)] leading-[1]",
                        scaleOnHover && "group/item"
                    )}
                    key={key}
                    role="listitem"
                >
                    {typeof item === "string" ? (
                        <span>{item}</span>
                    ) : (
                        item.node ?? item
                    )}
                </li>
            ),
            [scaleOnHover]
        );

        const logoLists = useMemo(() => {
            if (!logos || logos.length === 0) return null;

            const extendedLogos =
                direction === "left"
                    ? [logos[logos.length - 1], ...logos, logos[0]]
                    : [logos[0], ...logos, logos[logos.length - 1]];

            return Array.from({ length: copyCount }, (_, copyIndex) => (
                <ul
                    className={cx(
                        "flex items-center",
                        direction === "right" && "flex-row-reverse"
                    )}
                    key={`copy-${copyIndex}`}
                    role="list"
                    aria-hidden={copyIndex > 0}
                    ref={copyIndex === 0 ? seqRef : undefined}
                >
                    {extendedLogos.map((item, itemIndex) =>
                        renderLogoItem(item, `${copyIndex}-${itemIndex}`)
                    )}
                </ul>
            ));
        }, [copyCount, logos, renderLogoItem, direction]);

        const containerStyle = useMemo(
            () => ({
                width: toCssLength(width) ?? "100%",
                ...cssVariables,
                ...style,
            }),
            [width, cssVariables, style]
        );

        return (
            <div
                ref={containerRef}
                className={rootClasses}
                style={containerStyle}
                role="region"
                aria-label={ariaLabel}
                onMouseEnter={handleMouseEnter}
                onMouseLeave={handleMouseLeave}
            >
                <div
                    className={cx("flex w-max will-change-transform select-none")}
                    ref={trackRef}
                >
                    {logoLists}
                </div>
            </div>
        );
    }
);

LogoLoop.displayName = "LogoLoop";
export default LogoLoop;
