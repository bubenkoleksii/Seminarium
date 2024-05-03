'use client';

import { FC } from 'react';
import Link from 'next/link';
import { Logo } from '@/components/logo/Logo';
import { LanguageDropdown } from './language-dropdown';

import styles from './Navbar.module.scss';

const Navbar: FC = () => {
  return (
    <header className={styles.header}>
        <Link href="/" className={styles.logo}>
          <Logo />

          <h1 className={styles.logoText}>Seminarium</h1>
        </Link>

      <div>Middle</div>

      <div>
        <LanguageDropdown />
      </div>
    </header>
  );
};

export { Navbar };
