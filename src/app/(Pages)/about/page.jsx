'use client'
import React from 'react'
import { useSelector } from "react-redux"

const About = () => {
  let {counter} = useSelector((state) => state.counter)

  return <>
  <h1>About Page!</h1>
      <h2>Count : {counter} </h2>

  </>
}

export default About