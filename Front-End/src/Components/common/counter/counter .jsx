import React from 'react'
import { useSelector } from 'react-redux'
'use client'
const Counter = () => {
    let { counter } = useSelector((state) => state.counter)
    return (
        <div>counter : {counter} </div>
    )
}

export default Counter