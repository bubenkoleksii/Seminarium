import { FC } from "react";
import { Cloud } from 'lucide-react';
import Link from 'next/link';

const Sidebar: FC = () => {
  return (
    <div className="sidebar bg-white h-screen w-12 flex flex-col justify-start items-center bg-gray-100">
      <Link
        href="/"
        className="text-gray-800 w-full h-12 flex items-center justify-center hover:bg-gray-200 transition duration-300"
      >
        <Cloud color="#3B0764" size={20} />
      </Link>
      <Link
        href="/"
        className="text-gray-800 w-full h-12 flex items-center justify-center hover:bg-gray-200 transition duration-300"
      >
        <Cloud color="#3B0764" size={20} />
      </Link>
    </div>
  );
};

export { Sidebar };
