'use client'
import React from 'react'
import Image from 'next/image';
import logo from '../../../../public/assets/logo.svg'
import { BsFacebook } from "react-icons/bs";
import { FaTiktok } from "react-icons/fa6";
import { PiInstagramLogoFill } from "react-icons/pi";
import { PiYoutubeLogoFill } from "react-icons/pi";
import { useTranslation } from 'react-i18next';
import { useSearchParams } from 'next/navigation';
import { HiOutlineLocationMarker } from "react-icons/hi";
import { LuMail } from "react-icons/lu";
import { LuPhone } from "react-icons/lu";

const Footer = () => {
    const { t, i18n } = useTranslation(['footer']);
    const searchParams = useSearchParams();
    const lang = searchParams.get('lang') || i18n.language || 'en';
    const year = new Date().getFullYear();
    return <>

        <footer className="py-8 border border-t border-b-0 border-gray-400 text-go-primary-g">
            <div className="content container mx-auto grid gap-8 grid-cols-2 sm:grid-cols-3 md:grid-cols-4 lg:grid-cols-6
                            justify-items-start px-4 sm:w-full sm:px-0"
                            aria-label="Footer">
                                
                {/* logo + social */}
                <div className="flex flex-col justify-center items-center">
                    <div className="logo">
                        <Image src={logo} width={100} height={100} alt="app's logo" />
                    </div>
                    <div className="social-icons flex gap-1 mt-2 text-go-primary-g">
                        <BsFacebook size={18} />
                        <PiInstagramLogoFill size={18} />
                        <FaTiktok size={18} />
                        <PiYoutubeLogoFill size={18} />
                    </div>
                </div>

                {/* explore */}
                <div className="[&>h3]:text-go-primary-e [&>h3]:text-xl [&>h3]:mb-2 font-semibold">
                    <h3>{t('sections.0.title')}</h3>
                    <div className="content flex gap-3">
                        <div className="right [&>h5]:mb-1">
                            <h5>{t('sections.0.items.0')}</h5>
                            <h5>{t('sections.0.items.1')}</h5>
                            <h5>{t('sections.0.items.2')}</h5>
                        </div>
                        <div className="left [&>h5]:mb-1">
                            <h5>{t('sections.0.items.3')}</h5>
                            <h5>{t('sections.0.items.4')}</h5>
                        </div>
                    </div>
                </div>

                {/* signup */}
                <div className="[&>h3]:text-go-primary-e [&>h3]:text-xl [&>h3]:mb-2 font-semibold">
                    <h3>{t('sections.1.title')}</h3>
                    <div className='[&>h5]:mb-1'>
                        <h5>{t('sections.1.items.0')}</h5>
                        <h5>{t('sections.1.items.1')}</h5>
                    </div>
                </div>

                {/* help */}
                <div className="[&>h3]:text-go-primary-e [&>h3]:text-xl [&>h3]:mb-2 font-semibold">
                    <h3>{t('sections.2.title')}</h3>
                    <div className='[&>h5]:mb-1'>
                        <h5>{t('sections.2.items.0')}</h5>
                        <h5>{t('sections.2.items.1')}</h5>
                    </div>
                </div>

                {/* contact (ياخد باقي العرض) */}
                <div
                    className="[&>h3]:text-go-primary-e [&>h3]:text-xl font-semibold col-span-2 sm:col-span-2 md:col-span-3 lg:col-span-2 xl:col-sp 
                    [&>h3]:mb-2" >
                    <h3>{t('sections.3.title')}</h3>
                    <div className="flex flex-col gap-1">
                        <h5 className="flex items-center gap-1.5">
                            <HiOutlineLocationMarker size={20} /> {t('sections.3.items.0')}
                        </h5>
                        <h5 className="flex items-center gap-1.5">
                            <LuMail /> {t('sections.3.items.1')}
                        </h5>
                        <h5 className="flex items-center gap-1.5">
                            <LuPhone /> {t('sections.3.items.2')}
                        </h5>
                    </div>
                </div>
            </div>

            <div className="rights border border-b-0 mt-8 pt-2 border-gray-300 text-center text-go-primary-g">
                <p className={`${lang == 'ar' ? 'font-honor' : 'font-figtree'}`}>
                    {t('rights', { year })}
                </p>
            </div>
        </footer>
    </>
}

export default Footer;