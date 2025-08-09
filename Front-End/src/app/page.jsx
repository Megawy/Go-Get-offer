'use client'
import { Button } from "@/Components/chadcn-ui/button.jsx"
import { useSelector } from "react-redux"



export default function Home() {
  let { counter } = useSelector((state) => state.counter)
  return <>
    <h1>Hello From Home !</h1>
    <h2>Count : {counter} </h2>
    <div className="flex justify-center">
      <Button className='rounded-md'>Welcome</Button>
    </div>
  </>
}
