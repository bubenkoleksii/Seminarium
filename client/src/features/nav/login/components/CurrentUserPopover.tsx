'use client';

import { FC, Fragment } from 'react';
import { CurrentUser } from '../types';
import { Menu, Transition } from '@headlessui/react';
import { ChevronDownIcon } from '@heroicons/react/20/solid';
import Image from 'next/image';
import { LuLogOut } from 'react-icons/lu';
import { signOut } from 'next-auth/react';
import { useTranslations } from 'next-intl';
import { getDefaultProfileImgByType, truncateString } from '@/shared/helpers';
import { useProfiles, activate, useSchoolProfilesStore } from '@/features/user';
import { useIsMutating, useMutation } from '@tanstack/react-query';
import { toast } from 'react-hot-toast';

interface CurrentUserPopoverProps {
  user: CurrentUser;
}

const CurrentUserPopover: FC<CurrentUserPopoverProps> = ({ user }) => {
  const t = useTranslations();
  const { activeProfile, isLoading, isError } = useProfiles();
  const profiles = useSchoolProfilesStore((store) => store.profiles);
  const changeActiveProfile = useSchoolProfilesStore(
    (store) => store.changeActiveProfile,
  );

  const isMutating = useIsMutating();

  const { mutate: activateProfile } = useMutation({
    mutationFn: activate,
    mutationKey: ['activateProfile'],
    retry: 3,
    onSuccess: (response) => {
      if (response && response.error) {
        const errorMessages = {
          400: t('SchoolProfile.activateFail'),
        };

        toast.error(
          errorMessages[response.error.status] || t('labels.internal'),
        );
      } else {
        changeActiveProfile(response.id);
        toast.success(t('SchoolProfile.activateSuccess'), { duration: 3500 });
      }
    },
  });

  if ((isLoading || isError || isMutating) && user.role === 'user') return null;

  const handleLogout = () => {
    signOut({
      callbackUrl: '/api/signout',
      redirect: true,
    });
  };

  const adminProfileImage = '/profile/admin.png';
  const profileImage =
    user.role === 'admin'
      ? adminProfileImage
      : getDefaultProfileImgByType(activeProfile?.type);

  const handleChangeActiveProfile = (id: string) => {
    if (activeProfile.id === id) return;

    activateProfile(id);
  };

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
                  {user.name}{' '}
                  {user.role === 'admin' ? `(${t('CurrentUser.admin')})` : ''}
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

          {profiles && profiles?.length > 0 && (
            <div className="cursor-default">
              <Menu.Item>
                <div className="flex flex-col gap-2 p-2 text-xs">
                  <h6 className="text-center text-xs font-bold">
                    {t('SchoolProfile.smallListLabel')}
                  </h6>
                  {profiles.map((profile, idx) => (
                    <div
                      key={idx}
                      onClick={() => handleChangeActiveProfile(profile.id)}
                      className={`${profile.isActive ? 'bg-green-100' : ''} flex cursor-pointer items-center justify-between p-2`}
                    >
                      <div>
                        {profile.type && (
                          <span className="font-bold">
                            {truncateString(
                              t(`SchoolProfile.type.${profile.type}`),
                              5,
                            )}
                          </span>
                        )}

                        {profile.schoolName && (
                          <span className="ml-1">
                            ({truncateString(profile.schoolName, 5)})
                          </span>
                        )}
                      </div>

                      <input
                        type="radio"
                        className="form-radio h-4 w-4 cursor-pointer text-green-400"
                        checked={profile.isActive}
                        onChange={() => handleChangeActiveProfile(profile.id)}
                      />
                      <hr />
                    </div>
                  ))}
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
