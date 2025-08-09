'use client'
import React from 'react'
import { increment, decrement } from '@/Redux/Slices/counterSlice.js'
import { useDispatch, useSelector } from 'react-redux'

const Blog = () => {
  let { counter } = useSelector((state) => state.counter)
  let dispatch = useDispatch()
  return <>
    <div className="container p-2 w-full mx-auto  flex justify-center flex-col items-center">
      <h1>Blog Page!</h1>
      <div className="flex flex-col justify-center  ">
        <h1 className='text-center'>Click to increase the counter !</h1>
        <h2>Count : {counter}</h2>
        <button className='p-4 bg-sky-600 rounded-md cursor-pointer text-white'
          onClick={() => dispatch(increment())}
        >Increment</button>
        <button className='p-4 bg-sky-600 rounded-md cursor-pointer text-white'
          onClick={() => dispatch(decrement())}
        >Decrement</button>
      </div>
    </div>
  </>
}

export default Blog