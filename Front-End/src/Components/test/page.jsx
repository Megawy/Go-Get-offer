'use client'
import { useSelector } from "react-redux"




const Counter = () => {
    let { counter } = useSelector((state) => state.counter)
// const url =    process.env.NEXT_PUBLIC_BASE_URL
//     console.log(url)
  return <>
      <h2>Count : {counter} </h2>

  </>
}

export default Counter