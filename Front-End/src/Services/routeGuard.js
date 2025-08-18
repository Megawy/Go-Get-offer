"use client";
import { useEffect } from "react";
import { useRouter } from "next/navigation";
import useAuth from "@/Hooks/useAuth";

const routeGuard = ({ children }) => {
    const { isAuthenticated } = useAuth();
    const router = useRouter();

    useEffect(() => {
        if (!isAuthenticated) {
            router.push("/login");
        }
    }, [isAuthenticated, router]);

    if (!isAuthenticated) {
        return <p className="text-center mt-10">Checking authentication...</p>;
    }

    return children;
};

export default routeGuard;




// "use client";

// import { usePathname, useRouter } from "next/navigation";
// import { useSelector } from "react-redux";
// import { selectIsAuthenticated } from "@/Redux/Slices/authSlice";

// export default function ProtectedWrapper({ children }) {
//   const isAuthenticated = useSelector(selectIsAuthenticated);
//   const pathname = usePathname();
//   const router = useRouter();

//   // لو الصفحة هي landing page "/" → مفتوحة
//   if (pathname === "/") {
//     return children;
//   }

//   // لو مش authenticated → يروح على "/login"
//   if (!isAuthenticated) {
//     router.push("/login");
//     return null;
//   }

//   // لو authenticated → اعرض الصفحة
//   return children;
// }




// =====================================================================================

// "use client";

// import { usePathname, useRouter } from "next/navigation";
// import { useSelector } from "react-redux";
// import { selectIsAuthenticated } from "@/Redux/Slices/authSlice";
// import { routesConfig } from "@/Utils/routesConfig";

// export default function ProtectedWrapper({ children }) {
//   const isAuthenticated = useSelector(selectIsAuthenticated);
//   const pathname = usePathname();
//   const router = useRouter();

//   // نجيب config للصفحة الحالية
//   const route = routesConfig[pathname];
//   const isProtected = route?.isProtected ?? true; // لو مش موجود في config → protected

//   // لو الصفحة مش محمية → اعرض عادي
//   if (!isProtected) return children;

//   // لو authenticated وداخل على "/login" → رجعه للـ landing page
//   if (isAuthenticated && pathname === "/login") {
//     router.push("/");
//     return null;
//   }

//   // لو الصفحة محمية و مش authenticated → يروح login
//   if (!isAuthenticated) {
//     router.push("/login");
//     return null;
//   }

//   return children;
// }

//usage
// <ProtectedWrapper>
//   {children}  {/* أي صفحة هتمر على ال wrapper وتتطبق عليها rules */}
// </ProtectedWrapper>