import { NextResponse } from "next/server";

export function i18nMiddleware(req) {
    const url = req.nextUrl;
    const lang = url.searchParams.get("lang");

    if (!lang) {
        const cookieLang = req.cookies.get("Next-i18next")?.value || "en";

        const newUrl = url.clone();
        newUrl.searchParams.set("lang", cookieLang);

        return NextResponse.redirect(newUrl);
    }

    return NextResponse.next();
}
