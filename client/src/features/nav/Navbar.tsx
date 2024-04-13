'use client';

import { FC } from 'react';
import Link from 'next/link';
import { Logo } from '@/components/logo/Logo';
import { LanguageDropdown } from './components/language-dropdown';

import styles from './Navbar.module.scss';

const Navbar: FC = () => {
  return (
    <header className={styles.header}>
      <div className={styles.logo}>
        <Logo />

        <Link href="/">
          <h1 className={styles.logoText}>Seminarium</h1>
        </Link>
      </div>

      <div>Middle</div>

      <div>
        <LanguageDropdown />
      </div>
    </header>
  );
};

export { Navbar };
