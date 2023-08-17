import { ReactNode } from "react";
import Menu from "../components/menu/Menu";
import AlertPrinter from "../components/alerts/AlertPrinter";

const RootLayout = (props: { children: ReactNode }) => {
   return (
      <div className="flex flex-wrap min-[900px]:h-full">
         <aside className="grow basis-[250px] bg-slate-900 text-gray-100 pb-5">
            <Menu />
         </aside>
         <main className="grow-999 basis-[650px] min-inline-size-50 p-2">
            {props.children}
         </main>
         <AlertPrinter />
      </div>
   );
};

export default RootLayout;
