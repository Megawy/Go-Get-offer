
export const appRoles = {
    Client: "Client",
    Supplier: "Supplier",
    Admin: "Admin",
    User: "User"
}

// Utils/routeGate.js
export const routeGate = {
    public: [
        "/",
        "/login",
        "/sign-up",
    ],

    prefixes: [
        { prefix: "/client", roles: [appRoles.Client] },
        { prefix: "/supplier", roles: [appRoles.Supplier] },
        { prefix: "/admin", roles: [appRoles.Admin] },
    ],

    exceptions: [
        
        { path: "/supplier/profile", isProtected: true, roles: [appRoles.Client, appRoles.Admin] },
    ],
};

