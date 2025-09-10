'use client'
import Carousel from '@/Components/common/carousel/carousel';
import { useSearchParams } from 'next/navigation';
import React from 'react'
import { useTranslation } from 'react-i18next';
import car1En from '../../../../public/assets/carousel/CAROUSEL1.svg'
import car1Ar from '../../../../public/assets/carousel/CAROUSEL1-ar.svg'
import car2En from '../../../../public/assets/carousel/CAROUSEL2.svg'
import car2Ar from '../../../../public/assets/carousel/CAROUSEL2-ar.svg'
import car3En from '../../../../public/assets/carousel/CAROUSEL3.svg'
import car3Ar from '../../../../public/assets/carousel/CAROUSEL3-ar.svg'
import car4En from '../../../../public/assets/carousel/CAROUSEL4.svg'
import car4Ar from '../../../../public/assets/carousel/CAROUSEL4-ar.svg'
import car5En from '../../../../public/assets/carousel/CAROUSEL5.svg'
import car5Ar from '../../../../public/assets/carousel/CAROUSEL5-ar.svg'
import car6En from '../../../../public/assets/carousel/CAROUSEL6.svg'
import car6Ar from '../../../../public/assets/carousel/CAROUSEL6-ar.svg'

import highlightByWords from '@/Utils/helpers/wordHighlighter';

const images = [
    { en: car1En, ar: car1Ar, alt: { en: 'one trusted connection', ar: 'اتصال موثوق واحد' } },
    { en: car2En, ar: car2Ar, alt: { en: 'cash only for simplicity', ar: 'نقدًا فقط من أجل البساطة' } },
    { en: car3En, ar: car3Ar, alt: { en: 'fast and secure delivery', ar: 'توصيل سريع وآمن' } },
    { en: car4En, ar: car4Ar, alt: { en: '100 % payment security', ar: '100 % أمان الدفع' } },
    { en: car5En, ar: car5Ar, alt: { en: 'competitive pricing', ar: 'أسعار تنافسية' } },
    { en: car6En, ar: car6Ar, alt: { en: 'a unified marketplace', ar: 'سوق موحد' } },
]

const WhySection = () => {
    const { t, i18n } = useTranslation(["whyChoose"]);
    const searchParams = useSearchParams();
    const lang = searchParams.get("lang") || i18n.language || "en";

    return <>

        <section className=" py-12 bg-go-background-gr w-full flex flex-col items-center justify-center" aria-labelledby="why-choose-title">
            <div className="container">
                <div className="text-center mb-10">
                    <h1 className={`text-2xl md:text-4xl lg:text-5xl text-go-primary-g font-bold mb-7 ${lang === "ar" ? "font-honor" : "font-figtree"}`}> {highlightByWords(t("title"), [
                        {
                            word: "Go-GetOffer",
                            className:
                                `text-go-primary-e font-bavistage tracking-wide font-medium text-3xl md:text-6xl lg:text-6xl ${lang === "ar" ? "text-6xl!" : ""}`,
                        },
                    ])}</h1>
                    <h2 className="lg:text-2xl text-xl px-10 md:px-0 text-go-primary-g font-semibold mb-5">{t("subtitle")}</h2>
                </div>
                <div className="w-full max-w-4xl md:max-w-5xl mx-auto px-2 md:px-0 ">
                    <Carousel
                        images={images}
                        lang={lang}
                        autoplay={true}
                        autoplayInterval={2000}
                        isDraggable={true}
                        slidesPerView={1}
                        slidesToScroll={1}
                        loop={true}
                        parallaxStrength={50}
                    />
                </div>
            </div>
        </section>
    </>
}

export default WhySection