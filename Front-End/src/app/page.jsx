'use client'
import { useSelector } from "react-redux"



export default function Home() {
  let {counter} = useSelector((state) => state.counter)
  return <>
    <h1>Hello From Home !</h1>
    <h2>Count : {counter} </h2>
  </>
}
