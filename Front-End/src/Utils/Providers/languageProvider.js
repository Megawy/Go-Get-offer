'use client';

import { useEffect, useState } from "react";
import Cookies from "js-cookie";
import i18n from "../../lib/I18next/i18next";

export default function LanguageProvider({ children, defaultLang }) {
    const [mounted, setMounted] = useState(false);

    useEffect(() => {
        const cookieLang = Cookies.get("Next-i18next") || defaultLang; 
        if (i18n.language !== cookieLang) {
            i18n.changeLanguage(cookieLang);
        }
        setMounted(true);
    }, [defaultLang, i18n]);

    // منع أي mismatch render قبل mount
    if (!mounted) return null;

    return <>{children}</>;
}
