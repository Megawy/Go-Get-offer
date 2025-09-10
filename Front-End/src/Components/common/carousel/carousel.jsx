"use client"

import React, { useEffect, useRef, useCallback } from "react"
import useEmblaCarousel from "embla-carousel-react"
import Autoplay from "embla-carousel-autoplay"
import { DotButton, useDotButton } from "./dots"
import Image from "next/image"

const TWEEN_FACTOR_BASE = 0.2

export default function Carousel({
    lang = "en",
    images = [],
    parallaxStrength = 20,
    autoplay = false,
    autoplayInterval = 4000,
    isDraggable = true,
    slidesToScroll = 1,
    slidesPerView = 1,
    loop = true,
}) {
    const autoplayPlugin = useRef(
        Autoplay({ delay: autoplayInterval, stopOnInteraction: false })
    )

    const options = {
        loop,
        draggable: isDraggable,
        slidesToScroll,
        align: "start",
        direction: lang === "ar" ? "rtl" : "ltr",
    }

    const [emblaRef, emblaApi] = useEmblaCarousel(
        options,
        autoplay ? [autoplayPlugin.current] : []
    )

    const tweenFactor = useRef(0)
    const tweenNodes = useRef([])

    const { selectedIndex, scrollSnaps } = useDotButton(emblaApi)

    const setTweenNodes = useCallback((emblaApi) => {
        tweenNodes.current = emblaApi.slideNodes().map((slideNode) => {
            return slideNode.querySelector(".embla__parallax__layer")
        })
    }, [])

    const setTweenFactor = useCallback((emblaApi) => {
        tweenFactor.current = TWEEN_FACTOR_BASE * emblaApi.scrollSnapList().length
    }, [])

    const tweenParallax = useCallback(
        (emblaApi, eventName) => {
            if (parallaxStrength === 0) return

            const engine = emblaApi.internalEngine()
            const scrollProgress = emblaApi.scrollProgress()
            const slidesInView = emblaApi.slidesInView()
            const isScrollEvent = eventName === "scroll"

            emblaApi.scrollSnapList().forEach((scrollSnap, snapIndex) => {
                let diffToTarget = scrollSnap - scrollProgress
                const slidesInSnap = engine.slideRegistry[snapIndex]

                slidesInSnap.forEach((slideIndex) => {
                    if (isScrollEvent && !slidesInView.includes(slideIndex)) return

                    if (engine.options.loop) {
                        engine.slideLooper.loopPoints.forEach((loopItem) => {
                            const target = loopItem.target()
                            if (slideIndex === loopItem.index && target !== 0) {
                                const sign = Math.sign(target)
                                if (sign === -1) diffToTarget = scrollSnap - (1 + scrollProgress)
                                if (sign === 1) diffToTarget = scrollSnap + (1 - scrollProgress)
                            }
                        })
                    }

                    const direction = lang === "ar" ? 1 : -1
                    const translate =
                        diffToTarget * direction * tweenFactor.current * parallaxStrength

                    const tweenNode = tweenNodes.current[slideIndex]
                    if (tweenNode) {
                        tweenNode.style.transform = `translateX(${translate}%)`
                    }
                })
            })
        },
        [parallaxStrength, lang]
    )

    // handle dot click بحيث يظهر السلايد الصح مهما مكان الـ active
    const handleDotClick = useCallback(
        (visualIndex) => {
            if (!emblaApi) return
            const N = scrollSnaps.length
            if (N === 0) return

            const targetIndex = lang === "ar" ? N - 1 - visualIndex : visualIndex
            const currentIndex =
                emblaApi.selectedScrollSnap != null
                    ? emblaApi.selectedScrollSnap()
                    : selectedIndex

            if (loop) {
                const forwardSteps = (targetIndex - currentIndex + N) % N
                const backwardSteps = (currentIndex - targetIndex + N) % N

                if (forwardSteps <= backwardSteps) {
                    for (let i = 0; i < forwardSteps; i++) {
                        emblaApi.scrollNext()
                    }
                } else {
                    for (let i = 0; i < backwardSteps; i++) {
                        emblaApi.scrollPrev()
                    }
                }
                return
            }

            emblaApi.scrollTo(targetIndex)
        },
        [emblaApi, scrollSnaps, selectedIndex, lang, loop]
    )

    useEffect(() => {
        if (!emblaApi) return
        setTweenNodes(emblaApi)
        setTweenFactor(emblaApi)
        tweenParallax(emblaApi)

        emblaApi
            .on("reInit", setTweenNodes)
            .on("reInit", setTweenFactor)
            .on("reInit", tweenParallax)
            .on("scroll", tweenParallax)
            .on("slideFocus", tweenParallax)
    }, [emblaApi, tweenParallax, setTweenNodes, setTweenFactor])

    return (
        <div className="embla w-full" dir={lang === "ar" ? "rtl" : "ltr"}>
            {/* viewport */}
            <div className="overflow-hidden" ref={emblaRef}>
                {/* container */}
                <div className="flex touch-pan-y">
                    {images.map((img, index) => (
                        <div
                            className="embla__slide relative flex-[0_0_100%] min-w-0"
                            key={index}
                            style={{ flexBasis: `${100 / slidesPerView}%` }}
                        >
                            <div className="embla__parallax overflow-hidden rounded-2xl">
                                <div className="embla__parallax__layer will-change-transform">
                                    <Image
                                        src={
                                            typeof img[lang] === "string" ? img[lang] : img[lang].src
                                        }
                                        alt={img.alt[lang]}
                                        width={
                                            typeof img[lang] === "string"
                                                ? undefined
                                                : img[lang].width
                                        }
                                        height={
                                            typeof img[lang] === "string"
                                                ? undefined
                                                : img[lang].height
                                        }
                                        className="w-full h-auto object-contain"
                                        priority={index === 0}
                                    />
                                </div>
                            </div>
                        </div>
                    ))}
                </div>
            </div>

            {/* Dots */}
            <div className="flex justify-center items-center h-5 gap-1 mt-4">
                {scrollSnaps.map((_, index) => {
                    const dotIndex = lang === "en" ? scrollSnaps.length - 1 - index : index
                    const adjustedSelectedIndex =
                        lang === "ar"
                            ? scrollSnaps.length - 1 - selectedIndex
                            : selectedIndex

                    return (
                        <DotButton
                            key={index}
                            onClick={() => handleDotClick(dotIndex)}
                            className={`w-2 h-2 md:w-4 md:h-4 rounded-full transition-all duration-300 ${dotIndex === adjustedSelectedIndex
                                    ? "bg-go-primary-o w-4 h-4 md:h-6 md:w-6"
                                    : "bg-go-primary-g hover:bg-go-primary-e hover:w-6 hover:h-6 cursor-pointer"
                                }`}
                        />
                    )
                })}
            </div>
        </div>
    )
}
