import { redirect } from 'next/navigation';
import { useEffect } from 'react';
import { signIn } from 'next-auth/react';

export default function RootPage() {
  redirect('/uk');
}
