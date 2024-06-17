type PracticalLessonItemSubmitResponse = {
  id: string;
  createdAt: string;
  lastUpdatedAt?: string | null;
  studentId: string;
  studentName?: string | null;
  practicalLessonItemId: string;
  text?: string | null;
  attachments?: string[] | null;
  status: string;
};

type CreatePracticalLessonItemSubmitRequest = {
  practicalLessonItemId: string;
  text?: string | null;
};
