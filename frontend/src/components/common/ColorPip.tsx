const ColorPip = (props: { hexCodeCode: string, className?: string }) => {
   return (
       <div
       className={`w-2 h-2 rounded-full inline-block mx-1 leading-[0px] ${props.className ? props.className: ""}`}
       style={{ backgroundColor: props.hexCodeCode }}
       />
   );
};

export default ColorPip;
