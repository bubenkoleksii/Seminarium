import createMiddleware from 'next-intl/middleware';

export default createMiddleware({
  locales: ['uk', 'crh'],
  defaultLocale: 'uk',
});

export const config = {
  matcher: ['/', '/(uk|crh)/:path*'],
};
