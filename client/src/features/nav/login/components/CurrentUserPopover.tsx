import { FC, Fragment } from 'react';
import { CurrentUser } from '../types';
import { Menu, Transition } from '@headlessui/react';
import { ChevronDownIcon } from '@heroicons/react/20/solid';
import Image from 'next/image';
import { LuLogOut } from 'react-icons/lu';
import { signOut } from 'next-auth/react';
import { useTranslations } from 'next-intl';

interface CurrentUserPopoverProps {
  user: CurrentUser;
}

const CurrentUserPopover: FC<CurrentUserPopoverProps> = ({ user }) => {
  const t = useTranslations();

  const handleLogout = () => {
    signOut({
      callbackUrl: '/api/signout',
      redirect: true,
    });
  };

  const defaultProfileImages = {
    'admin': '/profile/admin.png',
    'student': '/profile/student.png',
    'class_teacher': '/profile/class_teacher.png',
    'school_admin': '/profile/school_admin.png',
    'teacher': '/profile/teacher.png',
    'parent': '/profile/parent.png',
  };

  const profileImage = defaultProfileImages[user.role] || '/profile/profile.svg';

  return (
    <Menu as="div" className="relative inline-block cursor-pointer text-left">
      <div>
        <Menu.Button
          className="inline-flex items-center justify-center rounded-md px-2 py-1 text-sm font-semibold
          text-gray-900 hover:bg-gray-200 focus:outline-none"
        >
          <Image
            src={profileImage}
            width={35}
            height={35}
            alt={`Profile image`}
            className="h-8 w-8 rounded-full"
          />
          <ChevronDownIcon
            className="h-4 w-4 text-gray-400"
            aria-hidden="true"
          />
        </Menu.Button>
      </div>

      <Transition
        as={Fragment}
        enter="transition ease-out duration-100"
        enterFrom="transform opacity-0 scale-95"
        enterTo="transform opacity-100 scale-100"
        leave="transition ease-in duration-75"
        leaveFrom="transform opacity-100 scale-100"
        leaveTo="transform opacity-0 scale-95"
      >
        <Menu.Items
          className="absolute right-0 z-10 mt-2 w-56 origin-top-right rounded-md bg-white shadow-lg ring-1
          ring-black ring-opacity-5 focus:outline-none"
        >
          <div className="cursor-default">
            <Menu.Item>
              <div className="p-2 text-xs font-bold">
                <p>
                  {user.name} {user.role === 'admin' ? `(${t('CurrentUser.admin')})` : ''}
                </p>
              </div>
            </Menu.Item>
          </div>

          {user.email && (
            <div className="cursor-default">
              <Menu.Item>
                <div className="flex p-2 text-xs">
                  <p>{user.email}</p>
                </div>
              </Menu.Item>
            </div>
          )}

          <Menu.Item>
            <div
              onClick={handleLogout}
              className="flex items-center gap-2 rounded-b-lg p-2 font-medium hover:bg-gray-100"
            >
              <div>{t('Auth.logoutBtn')}</div>
              <LuLogOut />
            </div>
          </Menu.Item>
        </Menu.Items>
      </Transition>
    </Menu>
  );
};

export { CurrentUserPopover };
