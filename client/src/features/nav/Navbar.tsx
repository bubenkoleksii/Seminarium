'use client';

import { FC } from 'react';
import Link from 'next/link';
import { Logo } from '@/components/logo/Logo';
import { LanguageDropdown } from './language-dropdown';
import { Login } from './login';

import styles from './Navbar.module.scss';
import { SessionProvider } from 'next-auth/react';
import { useNavStore } from './store/navStore';
import { useRouter, usePathname } from 'next/navigation';
import { useProfiles } from '@/features/user';
import { useLocale } from 'next-intl';

const Navbar: FC = () => {
  const activeLocale = useLocale();

  const sidebarOpen = useNavStore((store) => store.sidebarOpen);
  const setSidebarOpen = useNavStore((store) => store.setSidebarOpen);
  const pathname = usePathname();
  const router = useRouter();

  const { activeProfile, isLoading: profilesLoading } = useProfiles();

  if (profilesLoading) return null;

  const showSidebar = () => {
    const pathsWithSidebar = ['admin', 'u'];

    if (pathsWithSidebar.some((path) => pathname.includes(path)))
      setSidebarOpen(!sidebarOpen);
    else {
      router.push('/');
    }
  };

  const url = activeProfile ? `/${activeLocale}/u/` : '/';

  return (
    <header className={styles.header}>
      <SessionProvider>
        <div className={styles.logo}>
          <div onClick={showSidebar}>
            <Logo />
          </div>

          <Link href={url}>
            <h1 className={styles.logoText}>Seminarium</h1>
          </Link>
        </div>

        <div>Middle</div>

        <div className="flex items-center justify-center gap-4">
          <LanguageDropdown />
          <Login />
        </div>
      </SessionProvider>
    </header>
  );
};

export { Navbar };
