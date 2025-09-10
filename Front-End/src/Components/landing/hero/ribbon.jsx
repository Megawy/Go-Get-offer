import LogoLoop from '../../chadcn-ui/logo-loop/logo-loop';
import cat1 from '../../../../public/assets/buffet.svg';
import cat2 from '../../../../public/assets/food.svg';
import cat3 from '../../../../public/assets/stationary.svg';
import cat4 from '../../../../public/assets/packaging.svg';
import cat5 from '../../../../public/assets/cleaning.svg';
import cat6 from '../../../../public/assets/healthcare.svg';
import Image from 'next/image';
import { useTranslation } from "react-i18next";

export function Ribbon({ className }) {
    const { t, i18n } = useTranslation(["hero"]);
    const lang = i18n.language;

    // جلب الـ supplies_range من ملف الترجمة
    const suppliesRange = t("supplies_range", { returnObjects: true });
    const images = [
        cat1,
        cat2,
        cat3,
        cat4,
        cat5,
        cat6
    ]

    const techLogos = suppliesRange.map((title, index) => ({
        node: <RibbonItem key={index} title={title} img={images[index]} lang={lang} />
    }));

    return (
        <div
            style={{
                height: '200px',
                position: 'relative',
                overflow: 'hidden'
            }}
            className={className}
        >
            <LogoLoop
                logos={techLogos}
                speed={120}
                direction={lang === "ar" ? "right" : "left"}
                logoHeight={5}
                gap={60}
                pauseOnHover
                fadeOut
                fadeOutColor=""
                ariaLabel="Categories"
                className={`${i18n.language === "ar" ? "font-honor" : "font-figtree"}`}
            />
        </div>
    );
}


function RibbonItem({ title, img, lang }) {
    return (
        <div className="w-fit flex flex-col items-center justify-center">

            <div className="relative rounded-full bg-white
                        md:h-32 sm:h-28 h-24 
                        md:w-32 sm:w-28 w-24 
                        flex justify-center items-center 
                        transition-transform duration-300 overflow-hidden">
                <Image
                    src={img}
                    alt={title}
                    fill
                    className="object-fit"
                />
            </div>

            <p
                className={`text-white text-xl sm:text-2xl md:text-2xl
                    text-center mt-3 ${lang === "ar" ? "font-hacen" : "font-figtree"}`}
            >
                {title}
            </p>
        </div>
    );
}

export default RibbonItem;

