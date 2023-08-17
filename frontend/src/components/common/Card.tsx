import { ReactNode } from "react";

const Card = (props: { children: ReactNode; className?: string }) => {
   return (
      <section
         className={`border bg-white p-3 m-3 shadow ${
            props.className ? props.className : ""
         }`}
      >
         {props.children}
      </section>
   );
};

export default Card;
