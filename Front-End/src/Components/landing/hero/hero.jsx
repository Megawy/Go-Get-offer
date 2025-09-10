'use client'

import Link from 'next/link';
import React from 'react';
import heroBg from '../../../../public/assets/hero-bg.svg';
import { useTranslation } from 'react-i18next';
import { useSearchParams } from 'next/navigation';
import Image from 'next/image';
import highlightByWords from '@/Utils/helpers/wordHighlighter';    

import { Ribbon } from './ribbon';

const Hero = () => {
    const { t, i18n } = useTranslation(["hero"]);
    const searchParams = useSearchParams();
    const lang = searchParams.get("lang") || i18n.language || "en";

    // جلب الأزرار من JSON
    const buttons = t('buttons', { returnObjects: true });

    return (
        <header>
            <div className="relative">
                <div className="h-[90vh]">
                    <Image
                        src={heroBg}
                        alt="Hero Background"
                        fill
                        className="object-cover"
                        priority
                    />
                </div>
                <div className="absolute inset-0 bg-black/70 flex items-center justify-center">
                    <div className="text-center text-white py-8 sm:py-10 md:py-24 lg:py-14 container flex flex-col gap-12">
                        <section aria-labelledby='hero-title' className="">
                            {/* Title */}
                            <h1 id="hero-title"
                                className={`font-bold tracking-tighter text-4xl pt-10 md:text-5xl lg:text-6xl mb-4 leading-tight ${lang === "ar" ? "font-hacen" : "font-lemands tracking-wide"
                                    }`}
                            >
                                {/* Desktop */}
                                <span className="hidden lg:block whitespace-pre-line">
                                    {highlightByWords(t("title.desktop"), [
                                        { word: "Grow", className: "text-go-primary-e" },
                                        { word: "Together", className: "text-go-primary-o" },
                                        { word: "انموا", className: "text-go-primary-e" },
                                        { word: "معًا", className: "text-go-primary-o" },
                                    ])}
                                </span>

                                {/* Tablet */}
                                <span className="hidden md:block lg:hidden whitespace-pre-line">
                                    {highlightByWords(t("title.tablet"), [
                                        { word: "Grow", className: "text-go-primary-e" },
                                        { word: "Together", className: "text-go-primary-o" },
                                        { word: "انموا", className: "text-go-primary-e" },
                                        { word: "معًا", className: "text-go-primary-o" },
                                    ])}
                                </span>

                                {/* Mobile */}
                                <span className="block md:hidden whitespace-pre-line">
                                    {highlightByWords(t("title.mobile"), [
                                        { word: "Grow", className: "text-go-primary-e" },
                                        { word: "Together", className: "text-go-primary-o" },
                                        { word: "انموا", className: "text-go-primary-e" },
                                        { word: "معًا", className: "text-go-primary-o" },
                                    ])}
                                </span>
                            </h1>

                            {/* Subtitle */}
                            <p className={`lg:text-3xl md:px-0  px-6 tracking-tighter sm:text-2xl mb-6 ${lang === "ar" ? "xl:px-80 font-honor" : "xl:px-48 font-figtree"}`}>
                                {highlightByWords(t("subtitle"), [
                                    { word: "Go-GetOffer", className: "text-go-primary-e tracking-wide font-bavistage sm:text-4xl! lg:text-4xl!" },
                                ])}
                            </p>

                            {/* Buttons */}
                            <div className="flex justify-center gap-4 flex-wrap mb-2">
                                <Link
                                    href="#"
                                    className="bg-go-primary-e hover:bg-go-primary-o transition-all duration-300 ease-in-out text-white px-8 py-2 rounded-xl font-semibold text-lg"
                                >
                                    {buttons[0]}
                                </Link>
                                <Link
                                    href="#"
                                    className=" bg-white text-gray-950 hover:bg-black hover:text-white transition-all duration-300 ease-in-out px-6 py-2 rounded-xl font-semibold text-lg"
                                >
                                    {buttons[1]}
                                </Link>
                            </div>
                        </section>
                        <section aria-label='Hero supplies ribbon'>
                            <Ribbon />
                        </section>
                    </div>
                </div>
            </div>
        </header>
    );
};

export default Hero;
