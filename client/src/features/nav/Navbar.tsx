'use client';

import { FC } from 'react';

import { Logo } from '@/components/logo/Logo';

import styles from './Navbar.module.scss';

const Navbar: FC = () => {
  return (
    <header className={styles.header}>
      <div className={styles.logo}>
        <Logo />
        <h1 className={styles.logoText}>Seminarium</h1>
      </div>
      <div>Middle</div>
      <div>
        Right
      </div>
    </header>
  );
};

export { Navbar };
