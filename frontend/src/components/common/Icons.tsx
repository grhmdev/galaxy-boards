import {
   ViewColumnsIcon,
   ExclamationCircleIcon,
   FolderIcon,
   HomeIcon,
   PencilIcon,
   MagnifyingGlassCircleIcon,
   PlusSmallIcon,
   ChevronUpIcon,
   ChevronDownIcon,
   ArrowPathIcon,
   ArrowSmallRightIcon,
   XMarkIcon,
   CheckCircleIcon,
   TrashIcon,
   Bars2Icon,
} from "@heroicons/react/24/outline";

const defaultIconStyles = `h-[18px] w-[18px] inline-block`;
const iconWrapperStyles = "leading-[0px] inline-block";

interface IconProps {
   className?: string;
}

export const BoardIcon = ({ className }: IconProps) => {
   return (
      <div className={iconWrapperStyles}>
         <ViewColumnsIcon
            className={`${defaultIconStyles} ${className ? className : ""}`}
         />
      </div>
   );
};

export const TicketIcon = ({ className }: IconProps) => {
   return (
      <div className={iconWrapperStyles}>
         <ExclamationCircleIcon
            className={`${defaultIconStyles} ${className ? className : ""}`}
         />
      </div>
   );
};

export const DeleteIcon = ({ className }: IconProps) => {
   return (
      <div className={iconWrapperStyles}>
         <TrashIcon
            className={`${defaultIconStyles} ${className ? className : ""}`}
         />
      </div>
   );
};

export const TicketIconSearch = ({ className }: IconProps) => {
   return (
      <div className={iconWrapperStyles}>
         <MagnifyingGlassCircleIcon
            className={`${defaultIconStyles} ${className ? className : ""}`}
         />
      </div>
   );
};

export const ProjectIcon = ({ className }: IconProps) => {
   return (
      <div className={iconWrapperStyles}>
         <FolderIcon
            className={`${defaultIconStyles} ${className ? className : ""}`}
         />
      </div>
   );
};

export const OverviewIcon = ({ className }: IconProps) => {
   return (
      <div className={iconWrapperStyles}>
         <HomeIcon
            className={`${defaultIconStyles} ${className ? className : ""}`}
         />
      </div>
   );
};

export const EditIcon = ({ className }: IconProps) => {
   return (
      <div className={iconWrapperStyles}>
         <PencilIcon
            className={`${defaultIconStyles} ${className ? className : ""}`}
         />
      </div>
   );
};

export const AddIcon = ({ className }: IconProps) => {
   return (
      <div className={iconWrapperStyles}>
         <PlusSmallIcon
            className={`${defaultIconStyles} ${className ? className : ""}`}
         />
      </div>
   );
};

export const UpArrowIcon = ({ className }: IconProps) => {
   return (
      <div className={iconWrapperStyles}>
         <ChevronUpIcon
            className={`${defaultIconStyles} ${className ? className : ""}`}
         />
      </div>
   );
};

export const DownArrowIcon = ({ className }: IconProps) => {
   return (
      <div className={iconWrapperStyles}>
         <ChevronDownIcon
            className={`${defaultIconStyles} ${className ? className : ""}`}
         />
      </div>
   );
};

export const RefreshIcon = ({ className }: IconProps) => {
   return (
      <div className={iconWrapperStyles}>
         <ArrowPathIcon
            className={`${defaultIconStyles} ${className ? className : ""}`}
         />
      </div>
   );
};

export const CloseIcon = ({ className }: IconProps) => {
   return (
      <div className={iconWrapperStyles}>
         <XMarkIcon
            className={`${defaultIconStyles} ${className ? className : ""}`}
         />
      </div>
   );
};

export const WarningIcon = ({ className }: IconProps) => {
   return (
      <div className={iconWrapperStyles}>
         <ExclamationCircleIcon
            className={`${defaultIconStyles} ${className ? className : ""}`}
         />
      </div>
   );
};

export const SuccessIcon = ({ className }: IconProps) => {
   return (
      <div className={iconWrapperStyles}>
         <CheckCircleIcon
            className={`${defaultIconStyles} ${className ? className : ""}`}
         />
      </div>
   );
};

export const ArrowRightIcon = ({ className }: IconProps) => {
   return (
      <div className={iconWrapperStyles}>
         <ArrowSmallRightIcon
            className={`${defaultIconStyles} ${className ? className : ""}`}
         />
      </div>
   );
};

export const DragIcon = ({ className }: IconProps) => {
   return (
      <div className={iconWrapperStyles}>
         <Bars2Icon
            className={`${defaultIconStyles} ${className ? className : ""}`}
         />
      </div>
   );
};
