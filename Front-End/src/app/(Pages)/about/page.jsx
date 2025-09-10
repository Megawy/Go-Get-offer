import Counter from '@/Components/test/page';
import { staticPageMetadata } from '@/Utils/SEO/seo';
import React from 'react'

export async function generateMetadata() {
  return await staticPageMetadata({
    title: { en: "About Us", ar: "معلومات عنا" },
    description: {
      en: "Learn more about our company and mission.",
      ar: "تعرف على المزيد عن شركتنا ورسالتنا.",
    },
  });
}

export default function AboutPage() {
  return <>
  
    <h1>About Page!</h1>
    <Counter />

  </>

}

