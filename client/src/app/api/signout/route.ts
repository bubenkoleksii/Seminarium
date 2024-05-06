import { redirect } from 'next/navigation';
import { cookies } from 'next/headers';

export async function GET() {
  cookies().set(process.env.AUTH_ISSUER_COOKIE, '');
  redirect('/');
}
