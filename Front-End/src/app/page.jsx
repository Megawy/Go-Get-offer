import { Button } from "@/Components/chadcn-ui/button.jsx"
import { HoverCardBuilder } from "@/Components/chadcn-ui/hoverCard/hoverCard.jsx";
import Counter from "@/Components/test/page.jsx";
import UsersList from "@/Components/test2/page";
import { useFetch } from "@/Hooks/useFetch";
import store from "@/Redux/store";
import { Suspense } from "react";

// let usersResourses = useFetch('/users');

export default function Home() {
  // let users = usersResourses.read()

  // function GetUsers() {
  //   console.log(users)
  //   return <>

  //     <div className="flex justify-around">
  //       {users.map((user) => (
  //         <div
  //           key={user.id}
  //           className="card hover:shadow-2xl hover:scale-120 transition-all duration-300 shadow-lg p-5"
  //         >
  //           <h2>name: {user.name}</h2>
  //           <h2>id: {user.id}</h2>
  //         </div>
  //       ))}
  //     </div>
  //   </>
  // }



console.log(store.getState())


  return <>
    <h1>Hello From Home !</h1>
    <Counter />
    <div className="flex justify-center items-center flex-col">
      <Button  className='rounded-md p-2 w-2xs'>Welcome</Button>
    </div>
    {/* <Suspense fallback={<h1>Loading ... </h1>}>
      <GetUsers />
    </Suspense>

    <Suspense fallback={<p>Loading...</p>}>
      <UsersList />
    </Suspense> */}

    <Suspense fallback={<p>Loading Hover Card...</p>}>
      <UsersList />
    </Suspense>

    <HoverCardBuilder content={'this is a hover card !'} title={'GO-GETOFFER !'} />

  </>
}
