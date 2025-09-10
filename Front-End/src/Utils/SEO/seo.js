import { cookies } from "next/headers";

// 🛠 Helper: detect language from cookies (default = en)
export async function getLang() {
    const cookieStore = await cookies();
    return cookieStore.get("Next-i18next")?.value || "en";
}

// 🟢 Base Metadata (layout + Home page)
export async function homeMetadata() {
    const lang = await getLang();

    return {
        title: lang === "ar" ? "Go Get Offer|الرئيسية" : "Home|Go Get Offer",
        description:
            lang === "ar"
                ? "مرحبا بك فى موقعنا - استكشف المزيد"
                : "Welcome to our website - Explore more",
        icons: { icon: "/assets/logo-go.svg" },
        alternates: {
            canonical: "/", // ✅ النسخة الأساسية للـ Home
            languages: {
                en: "/?lang=en",
                ar: "/?lang=ar",
            },
        },
        openGraph: {
            type: "website",
            locale: lang,
            title: lang === "ar" ? "الصفحة الرئيسية" : "Home Page",
            description:
                lang === "ar"
                    ? "مرحبا بك فى موقعنا - استكشف المزيد"
                    : "Welcome to our website - Explore more",
            images: ["/og-default.png"],
        },
        twitter: {
            card: "summary_large_image",
            title: lang === "ar" ? "الصفحة الرئيسية" : "Home Page",
            description:
                lang === "ar"
                    ? "مرحبا بك فى موقعنا - استكشف المزيد"
                    : "Welcome to our website - Explore more",
            images: ["/og-default.png"],
        },
    };
}

// 📄 Static Page Metadata (About, Contact...)
export async function staticPageMetadata({ title, description, slug }) {
    const lang = await getLang();

    return {
        title: title[lang] || title.en,
        description: description[lang] || description.en,
        alternates: {
            canonical: `/${slug}`, 
            languages: {
                en: `/${slug}?lang=en`,
                ar: `/${slug}?lang=ar`,
            },
        },
        openGraph: {
            type: "website",
            locale: lang,
            title: title[lang] || title.en,
            description: description[lang] || description.en,
            images: ["/og-default.png"],
        },
        twitter: {
            card: "summary_large_image",
            title: title[lang] || title.en,
            description: description[lang] || description.en,
            images: ["/og-default.png"],
        },
    };
}
