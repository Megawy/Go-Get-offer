import { i18nMiddleware } from "./app/Middlewares/Localization.middleware";
import { NextResponse } from "next/server";

export function middleware(req) {
    const i18nResponse = i18nMiddleware(req);
    if (i18nResponse) return i18nResponse;

    return NextResponse.next(); 
}

export const config = {
    matcher: ["/((?!api|_next|.*\\..*).*)"],
};
