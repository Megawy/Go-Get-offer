import { CalendarIcon } from "lucide-react"

import {
    Avatar,
    AvatarFallback,
    AvatarImage,
} from "@/Components/chadcn-ui/avatar"
import { Button } from "@/Components/chadcn-ui/button"
import {
    HoverCard,
    HoverCardContent,
    HoverCardTrigger,
} from "@/Components/chadcn-ui/hover card/hover-card"

export function HoverCardBuilder({title , content , date , image}) {
    return (
        <HoverCard>
            <HoverCardTrigger asChild>
                <Button variant="link">{title || `@nextjs`}</Button>
            </HoverCardTrigger>
            <HoverCardContent className="w-80">
                <div className="flex justify-between gap-4">
                    <Avatar>
                        <AvatarImage src={image || "https://github.com/vercel.png"} />
                        <AvatarFallback>VC</AvatarFallback>
                    </Avatar>
                    <div className="space-y-1">
                        <h4 className="text-sm font-semibold">{title || `@nextjs`}</h4>
                        <p className="text-sm">
                            {content || `The React Framework – created and maintained by @vercel.`}
                        </p>
                        <div className="text-muted-foreground text-xs">
                            { date || `Joined December 2021`}
                        </div>
                    </div>
                </div>
            </HoverCardContent>
        </HoverCard>
    )
}
