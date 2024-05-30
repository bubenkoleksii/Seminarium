'use client';

import { useEffect, useState } from 'react';
import { useAdminStore } from '@/features/admin/store/adminStore';
import { useRouter } from 'next/navigation';
import { useSession } from 'next-auth/react';

export const useAuthRedirectByRole = (activeLocale, requiredRole = null) => {
  const { data: userData, status: userStatus } = useSession();
  const router = useRouter();
  const currentUser = userData?.user;
  const attempts = 2;
  const [attemptCount, setAttemptCount] = useState(0);
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    if (userStatus === 'loading') return;

    const handleRedirect = () => {
      if (userStatus === 'unauthenticated') {
        router.replace(`/${activeLocale}/access-denied/401`);
      } else if (requiredRole) {
        if (requiredRole === 'admin' && currentUser?.role !== 'admin') {
          router.replace(`/${activeLocale}/access-denied/403`);
        } else if (
          requiredRole === 'user' &&
          !(currentUser?.role === 'user' || currentUser?.role === 'admin')
        ) {
          router.replace(`/${activeLocale}/access-denied/403`);
        } else if (requiredRole === 'userOnly' && currentUser?.role !== 'user') {
          router.replace(`/${activeLocale}/access-denied/403`);
        }
      }
      setIsLoading(false);
    };

    if (
      userStatus === 'unauthenticated' ||
      (userStatus === 'authenticated' &&
        requiredRole &&
        ((requiredRole === 'admin' && currentUser?.role !== 'admin') ||
          (requiredRole === 'user' &&
            !(currentUser?.role === 'user' || currentUser?.role === 'admin')) ||
          (requiredRole === 'userOnly' && currentUser?.role !== 'user')
        ))
    ) {
      if (attemptCount < attempts) {
        setTimeout(() => {
          setAttemptCount(attemptCount + 1);
        }, 1000);
      } else {
        handleRedirect();
      }
    } else {
      setIsLoading(false);
    }
  }, [
    userStatus,
    currentUser,
    activeLocale,
    requiredRole,
    attemptCount,
    router,
  ]);

  useEffect(() => {
    if (
      userStatus === 'authenticated' &&
      (!requiredRole ||
        (requiredRole === 'admin' && currentUser?.role === 'admin') ||
        (requiredRole === 'user' &&
          (currentUser?.role === 'user' || currentUser?.role === 'admin')) ||
        (requiredRole === 'userOnly' && currentUser?.role === 'user')
      )
    ) {
      setIsLoading(false);
    }
  }, [userStatus, currentUser, requiredRole]);

  return {
    isUserLoading: userStatus === 'loading' || isLoading,
    isAuthenticated: userStatus === 'authenticated',
    user: currentUser,
  };
};

export const useSetCurrentTab = (currentTab) => {
  const setCurrentTab = useAdminStore((store) => store.setCurrentTab);

  useEffect(() => {
    setCurrentTab(currentTab);
  }, [setCurrentTab, currentTab]);
};

export const useScrollOffset = () => {
  const [topOffset, setTopOffset] = useState(0);

  const handleScroll = () => {
    const offset =
      window.pageYOffset ||
      document.documentElement.scrollTop ||
      document.body.scrollTop;

    setTopOffset(offset);
  };

  useEffect(() => {
    window.addEventListener('scroll', handleScroll);

    return () => {
      window.removeEventListener('scroll', handleScroll);
    };
  }, []);

  return topOffset;
};
