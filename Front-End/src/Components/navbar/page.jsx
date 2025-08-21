import Link from 'next/link.js'
import React from 'react'
import LogoutButton from '../logout-button/page'

const Navbar = () => {
    return <>
        <nav className="p-8 container mx-auto bg-gray-200">
            <ul className="flex gap-4 w-full justify-around">
                <li><Link className="px-6 py-2 bg-gray-400 rounded-md" href='/'>Home</Link></li>
                <li><Link className="px-6 py-2 bg-gray-400 rounded-md" href='/about'>About</Link></li>
                <li><Link className="px-6 py-2 bg-gray-400 rounded-md" href='/contact-us'>Contact Us</Link></li>
                <li><Link className="px-6 py-2 bg-gray-400 rounded-md" href='/blog'>Blog</Link></li>
                <li><Link className="px-6 py-2 bg-gray-400 rounded-md" href='/login'>Login</Link></li>
                <li><Link className="px-6 py-2 bg-gray-400 rounded-md" href='/sign-up'>SignUp</Link></li>
                <LogoutButton />
            </ul>
        </nav>
    </>
}

export default Navbar