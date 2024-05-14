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

const Navbar: FC = () => {
  const sidebarOpen = useNavStore((store) => store.sidebarOpen);
  const setSidebarOpen = useNavStore((store) => store.setSidebarOpen);
  const pathname = usePathname();
  const router = useRouter();

  const showSidebar = () => {
    const pathsWithSidebar = ['admin'];

    if (pathsWithSidebar.some((path) => pathname.includes(path)))
      setSidebarOpen(!sidebarOpen);
    else {
      router.push('/');
    }
  };

  return (
    <header className={styles.header}>
      <SessionProvider>
        <div className={styles.logo}>
          <div onClick={showSidebar}>
            <Logo />
          </div>

          <Link href="/">
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
