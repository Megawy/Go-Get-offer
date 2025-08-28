"use client";

import {
    Dialog,
    DialogContent,
    DialogHeader,
    DialogTitle,
    DialogDescription,
    DialogFooter,
} from "@/Components/chadcn-ui/modal/dialog";
import { Button } from "@/Components/chadcn-ui/button";
import { useModal } from "@/Hooks/useModal";
import { ModalActions } from "@/Hooks/useModal";
import { useRouter } from "next/navigation";

export default function GlobalModal() {
    const { modal, closeModal } = useModal();
    const {
        isOpen,
        type,
        title,
        message,
        image,
        actionType,
        confirmAction,
        cancelAction,
    } = modal;

    const router = useRouter();

    const typeColors = {
        success: "text-green-600",
        failure: "text-red-600",
        warning: "text-yellow-600",
        confirmation: "text-blue-600",
        info: "text-gray-600",
    };

    // Handle Confirm
    const handleConfirm = () => {
        if (actionType === ModalActions.RESET_PASSWORD) {
            router.push("/reset-password");
        }
        if (actionType === ModalActions.LOGIN) {
            router.push("/login");
        }


        // Optional: run custom confirmAction if provided
        if (typeof confirmAction === "function") {
            confirmAction();
        }

        closeModal();
    };

    // Handle Cancel
    const handleCancel = () => {
        if (typeof cancelAction === "function") {
            cancelAction();
        }
        closeModal();
    };

    return (
        <Dialog open={isOpen} onOpenChange={closeModal}>
            <DialogContent
                className="
                            fixed left-1/2 top-1/2 -translate-x-1/2 -translate-y-1/2
                            bg-white rounded-2xl shadow-lg w-full max-w-md
                            animate-in fade-in-0 zoom-in-95
                            data-[state=closed]:animate-out data-[state=closed]:fade-out-0 data-[state=closed]:zoom-out-95"
            >
                <DialogHeader className="flex flex-col items-center gap-4">
                    {image && <img src={image} alt={type} className="w-16 h-16 object-contain" />}
                    <DialogTitle className={`text-xl font-bold ${typeColors[type] || "text-gray-600"}`}>
                        {type?.charAt(0).toUpperCase() + type?.slice(1)}
                    </DialogTitle>
                    <DialogDescription className="text-center">{message}</DialogDescription>
                </DialogHeader>

                <DialogFooter className="flex justify-center gap-2">
                    {cancelAction && (
                        <Button variant="outline" onClick={handleCancel}>
                            Cancel
                        </Button>
                    )}
                    <Button onClick={handleConfirm}>OK</Button>
                </DialogFooter>
            </DialogContent>

        </Dialog>
    );
}
