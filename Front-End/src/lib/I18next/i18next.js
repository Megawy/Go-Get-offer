'use client'
import i18n from "i18next";
import { initReactI18next } from "react-i18next";
import LanguageDetector from "i18next-browser-languagedetector";

// ✅ Translation files
import enNavbar from "../../../public/localization/en/navbar.json";
import arNavbar from "../../../public/localization/ar/navbar.json";
import enHero from "../../../public/localization/en/hero-section.json";
import arHero from "../../../public/localization/ar/hero-section.json";
import enApproach from "../../../public/localization/en/approach.json";
import arApproach from "../../../public/localization/ar/approach.json";
import enFeatures from "../../../public/localization/en/features.json"
import arFeatures from "../../../public/localization/ar/features.json"
import enWhyChoose from "../../../public/localization/en/why-choose.json"
import arWhyChoose from "../../../public/localization/ar/why-choose.json"
import enCustomerRibbon from "../../../public/localization/en/customer-ribbon.json"
import arCustomerRibbon from "../../../public/localization/ar/customer-ribbon.json"
import enBlogLanding from "../../../public/localization/en/blog-landing.json"
import arBlogLanding from "../../../public/localization/ar/blog-landing.json"
import enJoinUs from "../../../public/localization/en/join-us-card.json"
import arJoinUs from "../../../public/localization/ar/join-us-card.json"
import enFooter from "../../../public/localization/en/footer.json"
import arFooter from "../../../public/localization/ar/footer.json"
// Helper: update direction
const updateHtmlDirection = (lng) => {
    if (typeof document !== "undefined") {
        document.documentElement.lang = lng;
        document.documentElement.dir = lng === "ar" ? "rtl" : "ltr";
    }
};

i18n
    .use(LanguageDetector)
    .use(initReactI18next)
    .init({
        resources: {
            en: {
                navbar: enNavbar, hero: enHero, approach: enApproach, features: enFeatures, whyChoose: enWhyChoose, customerRibbon: enCustomerRibbon, blogLanding: enBlogLanding,
                joinUs: enJoinUs, footer: enFooter
            },
            ar: {
                navbar: arNavbar, hero: arHero, approach: arApproach, features: arFeatures, whyChoose: arWhyChoose, customerRibbon: arCustomerRibbon, blogLanding: arBlogLanding,
                joinUs: arJoinUs, footer: arFooter
            }
        },
        fallbackLng: "en",
        interpolation: {
            escapeValue: false,
        },
        detection: {
            order: ["cookie", "htmlTag", "localStorage", "navigator"],
            caches: ["cookie"],
            lookupCookie: "Next-i18next", // Changed to match i18next's default
            cookieName: "Next-i18next", // Changed to match i18next's default
            cookieMinutes: 60 * 24 * 30,
        },
    });

// ✅ Sync direction initially + on language change
updateHtmlDirection(i18n.language);
i18n.on("languageChanged", updateHtmlDirection);

export default i18n;
