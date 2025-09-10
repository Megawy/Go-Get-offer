
import { Button } from "@/Components/chadcn-ui/button.jsx"
import { HoverCardBuilder } from "@/Components/chadcn-ui/hoverCard/hoverCard.jsx";
import Counter from "@/Components/test/page.jsx";
import UsersList from "@/Components/test2/page";
import { useFetch } from "@/Hooks/useFetch";
import store from "@/Redux/store";
import { homeMetadata } from "@/Utils/SEO/seo";
import { Suspense } from "react";
import Hero from "@/Components/landing/hero/hero";
import Approach from "@/Components/landing/approach/approach";
import Features from "@/Components/landing/features/features";
import WhySection from "@/Components/landing/why-choose/why-secton";
import CustomerRibbon from "@/Components/landing/customer-service/customer-ribbon";
import Blog from "@/Components/landing/blog/blog";
import JoinUsCard from "@/Components/landing/join-us/join-us-card";
// let usersResourses = useFetch('/users');

export async function generateMetadata() {
  return await homeMetadata();
}

export default function Home() {

  return (
    <>
      {/* header */}
      <Hero />
      {/* approach */}
      <Approach />
      {/* Features (Supplier) */}
      <Features personType="supplier" />
      <Features personType="client" />  
      {/* why Choose Go Get Offer */}
      <WhySection />
      {/* customer service ribbon */}
          <CustomerRibbon />
      {/* blog section */}
      <Blog />
      {/* join us */}
      <JoinUsCard/>
    </>
  );
}
