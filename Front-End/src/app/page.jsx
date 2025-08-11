
import { Button } from "@/Components/chadcn-ui/button.jsx"
import Counter from "@/Components/test/page.jsx";
import axiosRequester from "@/lib/Axios/axios.js"
import { use } from "react";



export default function Home() {

  async function getUsers() {
    const res = await axiosRequester.get();
    console.log(res.data)
    return res.data;
  }
  const users = use(getUsers());
  return <>
    <h1>Hello From Home !</h1>
    <Counter />
    <div className="flex justify-center items-center flex-col">
      <Button className='rounded-md p-2 w-2xs'>Welcome</Button>
      <div className="flex justify-around">
        {users.map((user) => (
          <div
            key={user.id}
            className="card hover:shadow-2xl hover:scale-120 transition-all duration-300 shadow-lg p-5"
          >
            <h2>name: {user.name}</h2>
            <h2>id: {user.id}</h2>
          </div>
        ))}
      </div>
    </div>
  </>
}
