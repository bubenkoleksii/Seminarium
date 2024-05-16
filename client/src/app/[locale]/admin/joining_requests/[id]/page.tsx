import { JoiningRequest } from '@/features/admin';

type Props = {
  params: {
    id: string;
  };
};

export default function JoiningRequestPage({ params }: Props) {
  return (
    <div className="p-3">
      <JoiningRequest id={params.id} />
    </div>
  );
}
