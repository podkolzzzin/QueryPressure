import React, { useState } from 'react';
import { useTranslation } from 'react-i18next';
import './file-uploader.css';
import { UploadIcon } from '@/assets/Icons';

interface FileUploadProps {
  onFileUpload: (file: File) => void;
  allowedFileTypes: string[];
}

export function FileUpload({ onFileUpload, allowedFileTypes }: FileUploadProps) {
  const { t } = useTranslation();
  const [selectedFile, setSelectedFile] = useState<File | null>(null);

  const fileTypes = allowedFileTypes.map((type) => `.${type}`).join(',');

  function handleFileChange(event: React.ChangeEvent<HTMLInputElement>) {
    const file = event.target.files && event.target.files[0];
    
    const extension = file?.name.split('.').pop();
    if(extension && fileTypes.includes(extension))
    {
        setSelectedFile(file);
    }
  };

  function handleUpload() {
    if (selectedFile) {
      onFileUpload(selectedFile);
      setSelectedFile(null);
    }
  };

  const isUploadDisabled = !selectedFile;

  return (
    <div className="file-upload">
      <div className="input-group">
        <input type="file" id="file" onChange={handleFileChange} accept={fileTypes} required/>
        <label htmlFor="file" className={`file-upload-label form-control${!isUploadDisabled ? ' disabled' : ''}`}>
          <p>
          {selectedFile ? selectedFile.name : t('labels.selectFile')}
          </p>
        </label>
        <button className="file-upload-button btn btn-outline-secondary" onClick={handleUpload} disabled={isUploadDisabled}>
          <UploadIcon />
        </button>
      </div>
    </div>
  );
}