import { ReactNode } from "react";

const LabelledContent = (props: { label: string; content: ReactNode }) => {
   return (
      <div className="p-1">
         <span className="uppercase text-xs">{props.label}</span>
         <div>{props.content}</div>
      </div>
   );
};

export default LabelledContent;
