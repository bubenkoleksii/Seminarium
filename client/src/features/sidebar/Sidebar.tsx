import { FC } from 'react';
import { Cloud } from 'lucide-react';
import Link from 'next/link';

const Sidebar: FC = () => {
  return (
    <div className="sidebar flex h-screen w-12 flex-col items-center justify-start bg-gray-100 bg-white">
      <Link
        href="/"
        className="flex h-12 w-full items-center justify-center text-gray-800 transition duration-300 hover:bg-gray-200"
      >
        <Cloud color="#3B0764" size={20} />
      </Link>
      <Link
        href="/"
        className="flex h-12 w-full items-center justify-center text-gray-800 transition duration-300 hover:bg-gray-200"
      >
        <Cloud color="#3B0764" size={20} />
      </Link>
    </div>
  );
};

export { Sidebar };
