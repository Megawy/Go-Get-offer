'use client'
import { useQueryFetch } from "@/Hooks/useQueryFetch";

export default function UsersList() {
    const { data } = useQueryFetch(["users"], "/users");

    return (
        <ul>
            {data.map(user => (
                <li key={user.id}>{user.name}</li>
            ))}
        </ul>
    )
}
