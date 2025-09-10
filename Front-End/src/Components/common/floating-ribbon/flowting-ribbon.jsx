'use client';
import React, { useRef, useEffect, useState } from 'react';
import { gsap } from 'gsap';

function FlowingMenu({ items = [], lang = 'en' }) {
    return (
        <div className="w-full h-full overflow-hidden">
            <nav className="flex flex-col h-full m-0 p-0">
                {items.map((item, idx) => (
                    <MenuItem key={idx} {...item} lang={lang} />
                ))}
            </nav>
        </div>
    );
}

function MenuItem({ link, text, image, lang }) {
    const containerRef = useRef(null);
    const fixedTextRef = useRef(null);
    const trackRef = useRef(null);

    const [repeatedContent, setRepeatedContent] = useState([]);
    const [singleItemWidth, setSingleItemWidth] = useState(0);

    const speed = 100; // px/sec

    useEffect(() => {
        gsap.set(fixedTextRef.current, { height: '100%', overflow: 'hidden' });
    }, []);

    useEffect(() => {
        if (!trackRef.current) return;

        const track = trackRef.current;
        const parent = track.parentElement;

        const minWidth = parent.offsetWidth * 3; 
        const tempContent = [];
        let i = 0;

        while (i < 20) {
            const el = (
                <div
                    key={i}
                    className={`flex items-center whitespace-nowrap ${lang === 'ar' ? 'mr-16' : 'mr-8'}`}
                >
                    <span className="text-white uppercase font-normal text-[4vh] leading-[1.2] p-[1vh_1vw_0]">
                        {text}
                    </span>
                    <div
                        className={`w-[200px] h-[7vh] my-[2em] ${lang === 'ar' ? 'mx-[4vw]' : 'mx-[2vw]'} rounded-[50px] object-center bg-cover`}
                        style={{ backgroundImage: `url(${image?.src || image})` }}
                    />
                </div>
            );
            tempContent.push(el);
            i++;
        }

        setRepeatedContent(tempContent);

        setTimeout(() => {
            if (trackRef.current && trackRef.current.children.length >= 2) {
                const firstChild = trackRef.current.children[0];
                const secondChild = trackRef.current.children[1];

                const firstChildLeft = firstChild.offsetLeft;
                const secondChildLeft = secondChild.offsetLeft;

                // المسافة الحقيقية بين العناصر
                const actualDistance = Math.abs(secondChildLeft - firstChildLeft);
                setSingleItemWidth(actualDistance);
            }
        }, 100);
    }, [text, image, lang]);

    // Loop animation
    useEffect(() => {
        if (!trackRef.current || repeatedContent.length === 0 || singleItemWidth === 0) return;

        let offset = 0;
        let rafId;

        const step = () => {
            const delta = speed / 60;
            offset += delta;

            if (offset >= singleItemWidth) {
                offset -= singleItemWidth;
            }

            trackRef.current.style.transform = `translateX(${-offset}px)`;
            rafId = requestAnimationFrame(step);
        };

        rafId = requestAnimationFrame(step);
        return () => cancelAnimationFrame(rafId);
    }, [repeatedContent, singleItemWidth]);

    // Hover effect
    const handleMouseEnter = () => {
        gsap.to(fixedTextRef.current, { height: 0, duration: 0.4, ease: 'expo.inOut' });
    };
    const handleMouseLeave = () => {
        gsap.to(fixedTextRef.current, { height: '100%', duration: 0.4, ease: 'expo.inOut' });
    };

    return (
        <div
            ref={containerRef}
            className="relative flex-1 overflow-hidden text-center"
            onMouseEnter={handleMouseEnter}
            onMouseLeave={handleMouseLeave}
        >
            {/* النص الثابت */}
            <a
                href={link}
                ref={fixedTextRef}
                className="absolute inset-0 flex items-center justify-center bg-go-primary-g text-white uppercase font-semibold text-[4vh] z-20 cursor-pointer"
            >
                {text}
            </a>

            {/* Marquee */}
            <div className="absolute inset-0 flex items-center bg-go-background-b overflow-hidden pointer-events-none z-10">
                <div ref={trackRef} className="flex flex-nowrap will-change-transform">
                    {repeatedContent}
                </div>
            </div>
        </div>
    );
}

export default FlowingMenu;