import { MouseEvent, ReactNode } from "react";

export const Button = (props: {
   children: ReactNode;
   title: string;
   className?: string;
   isSubmit?: boolean;
   disabled?: boolean;
   onClick?: (event: MouseEvent) => void;
}) => {
   const buttonClasses = `inline-block mr-1 p-1 border rounded bg-gradient-to-b from-gray-100 to-gray-200 border-gray-300 ${
      props.className ? props.className : ""
   } ${
      props.disabled === true
         ? "text-gray-300"
         : "text-black hover:border-gray-400"
   }`;

   return (
      <button
         className={`${buttonClasses} ${
            props.className ? props.className : ""
         }`}
         onClick={props.onClick}
         title={props.title}
         type={props.isSubmit ? "submit" : "button"}
         disabled={props.disabled === true ? true : false}
      >
         {props.children}
      </button>
   );
};

export const IconButton = (props: {
   children: ReactNode;
   title: string;
   className?: string;
   isSubmit?: boolean;
   disabled?: boolean;
   onClick?: (event: MouseEvent) => void;
}) => {
   const buttonClasses = `rounded leading-[0px] inline-block mr-1 p-1 ${
      props.className ? props.className : ""
   } ${
      props.disabled === true ? "text-gray-300" : "text-black hover:bg-gray-100"
   }`;

   return (
      <button
         className={`${buttonClasses} ${
            props.className ? props.className : ""
         }`}
         onClick={props.onClick}
         title={props.title}
         type={props.isSubmit ? "submit" : "button"}
         disabled={props.disabled === true ? true : false}
      >
         {props.children}
      </button>
   );
};
