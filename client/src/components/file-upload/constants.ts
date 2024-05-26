export const ALLOWED_IMAGE_EXTENSIONS = ['jpg', 'jpeg', 'png', 'gif'];
export const ALLOWED_FILE_EXTENSIONS = [
  'pdf',
  'doc',
  'docx',
  'txt',
  'xls',
  'xlsx',
  'ppt',
  'pptx',
  'csv',
  'zip',
  'rar',
  '7z',
  'tar',
  'gz',
  'mp3',
  'wav',
  'mp4',
  'avi',
  'mkv',
  ...ALLOWED_IMAGE_EXTENSIONS,
];

export const MAX_IMAGE_FILE_SIZE = 10 * 1024 * 1024;
export const MAX_FILE_SIZE = 50 * 1024 * 1024;
