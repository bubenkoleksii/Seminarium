import createMiddleware from 'next-intl/middleware';

export default createMiddleware({
  locales: ['uk', 'crh', 'ro', 'hu'],
  defaultLocale: 'uk',
  localeDetection: false,
});

export const config = {
  matcher: ['/', '/(uk|crh|ro|hu)/:path*'],
};
