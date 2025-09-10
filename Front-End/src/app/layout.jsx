import "./globals.css"; // in the highest order
import { Geist, Geist_Mono } from "next/font/google";
import Navbar from "@/Components/common/navbar/page.jsx";
import ReduxProvider from "@/Utils/Providers/reduxProvider";
import ReactQueryProvider from "@/Utils/Providers/reactQueryProvider";
import AppInitializer from "@/Utils/Providers/appInitializer";
import RouteGuard from "@/Services/routeGuard";
import GlobalModal from "@/Components/chadcn-ui/modal/customizableModal";
import LanguageProvider from "@/Utils/Providers/languageProvider"; // Client wrapper
import { cookies } from 'next/headers';
import { homeMetadata } from "@/Utils/SEO/seo";
import localFont from "next/font/local";
import Footer from "@/Components/common/footer/page";
// const geistSans = Geist({ variable: "--font-geist-sans", subsets: ["latin"] });
// const geistMono = Geist_Mono({ variable: "--font-geist-mono", subsets: ["latin"] });

const figtreeRegular = localFont({
  src: "../fonts/figtree-regular.ttf",
  variable: "--font-figtree",
  display: "swap",
});

const hacen = localFont({
  src: "../fonts/alfont_com_Hacen-Tunisia.ttf",
  variable: "--font-hacen",
  display: "swap",
});

const lemands = localFont({
  src: "../fonts/LemandsBold-DOXAm.ttf",
  variable: "--font-lemands",
  display: "swap",
});

const honor = localFont({
  src: "../fonts/HONORSansArabicUI-R.ttf",
  variable: "--font-honor",
  display: "swap",
});

const bavistage = localFont({
  src: "../fonts/bavistage-rpewe.otf",
  variable: "--font-bavistage",
  display: "swap",
});

const ibm = localFont({
  src: "../fonts/IBMPlexSansArabic-Regular.ttf",
  variable: "--font-ibm",
  display: "swap",
});

const roboto = localFont({
  src: "../fonts/RobotoFlex-VariableFont_GRAD,XOPQ,XTRA,YOPQ,YTAS,YTDE,YTFI,YTLC,YTUC,opsz,slnt,wdth,wght.ttf",
  variable: "--font-roboto",
  display: "swap",
});

export async function generateMetadata() {
  return {
    ...(await homeMetadata()),
    // will be set when the app is deployed , it's relevant warning should be ignored
    // metadataBase: new URL(process.env.NEXT_PUBLIC_BASE_URL),  
    title: "Get Offer - Main Layout",
  };
}

export default async function RootLayout({ children }) {
  // Get language from cookies on server side
  const cookieStore = await cookies();
  const lang = cookieStore.get('Next-i18next')?.value || 'en';
  const dir = lang === 'ar' ? 'rtl' : 'ltr';

  return (
    <html 
      suppressHydrationWarning 
      lang={lang} 
      dir={dir}
      key={lang}
      className={lang == 'ar' ? 'font-honor' : 'font-figtree'}
    >
      <body className={` ${figtreeRegular.variable} ${lemands.variable} ${hacen.variable} ${honor.variable} ${bavistage.variable} ${ibm.variable} ${roboto.variable} antialiased`}>
        <ReduxProvider>
          <ReactQueryProvider>
            <AppInitializer>
              <RouteGuard>
                <LanguageProvider defaultLang={lang}>
                  <Navbar />
                  {children}
                  <Footer/>
                  <GlobalModal />
                </LanguageProvider>
              </RouteGuard>
            </AppInitializer>
          </ReactQueryProvider>
        </ReduxProvider>
      </body>
    </html>
  );
}
