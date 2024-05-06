'use client';

import { FC } from 'react';
import Link from 'next/link';
import { Logo } from '@/components/logo/Logo';
import { LanguageDropdown } from './language-dropdown';
import { Login } from './login';

import styles from './Navbar.module.scss';
import { SessionProvider } from 'next-auth/react';

const Navbar: FC = () => {
  return (
    <header className={styles.header}>
      <SessionProvider>
        <Link href="/" className={styles.logo}>
          <Logo />

          <h1 className={styles.logoText}>Seminarium</h1>
        </Link>

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
