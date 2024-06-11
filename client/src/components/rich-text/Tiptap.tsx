'use client';

import Blockquote from '@tiptap/extension-blockquote';
import CharacterCount from '@tiptap/extension-character-count';
import { Color } from '@tiptap/extension-color';
import Gapcursor from '@tiptap/extension-gapcursor';
import HardBreak from '@tiptap/extension-hard-break';
import Link from '@tiptap/extension-link';
import TextAlign from '@tiptap/extension-text-align';
import TextStyle from '@tiptap/extension-text-style';
import Underline from '@tiptap/extension-underline';
import Youtube from '@tiptap/extension-youtube';
import { EditorContent, useEditor } from '@tiptap/react';
import StarterKit from '@tiptap/starter-kit';
import { useLocale, useTranslations } from 'next-intl';
import { Toolbar } from './Toolbar';

const Tiptap = ({ onChange, content, limit }: any) => {
  const t = useTranslations('Tiptap');
  const activeLocale = useLocale();

  const handleChange = (newContent: string) => {
    onChange(newContent);
  };

  const editor = useEditor({
    extensions: [
      StarterKit,
      Underline,
      Blockquote,
      Gapcursor,
      HardBreak,
      Color,
      TextStyle,
      CharacterCount.configure({
        limit,
      }),
      Link.configure({
        openOnClick: false,
        autolink: true,
      }),
      Youtube.configure({
        progressBarColor: 'white',
        interfaceLanguage: activeLocale || 'uk',
      }),
      TextAlign.configure({
        types: ['heading', 'paragraph'],
        alignments: ['left', 'center', 'right', 'justify'],
        defaultAlignment: 'left',
      }),
    ],
    content: content,
    autofocus: true,
    editorProps: {
      attributes: {
        class:
          'flex flex-col px-4 py-3 justify-start border-b border-r border-l border-gray-200 text-gray-400 items-start w-full gap-3' +
          ' ' +
          'font-medium text-[16px] pt-4 rounded-bl-md rounded-br-md outline-none',
      },
    },
    onUpdate: ({ editor }) => {
      handleChange(editor.getHTML());
    },
  });

  return (
    <div
      className="w-full"
      style={{ wordWrap: 'break-word', overflowWrap: 'break-word' }}
    >
      <Toolbar editor={editor} />
      <EditorContent
        style={{
          whiteSpace: 'pre-line',
          wordWrap: 'break-word',
          overflowWrap: 'break-word',
        }}
        editor={editor}
      />

      <div className="character-count flex flex-col items-end pl-3 pr-3 pt-3 text-sm text-gray-500">
        {editor?.storage.characterCount.characters()}/{limit} {t('symbols')}
        <br />
        {editor?.storage.characterCount.words()} {t('words')}
      </div>
    </div>
  );
};

export { Tiptap };
