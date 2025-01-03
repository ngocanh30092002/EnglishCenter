import React, { Suspense } from 'react';
import { Route, Routes, useLocation, useNavigate } from 'react-router-dom';
import LoaderPage from './../LoaderComponent/LoaderPage';
import MenuContextPage from '../MenuContextComponent/MenuContextPage';

const LazyLoading = (importFunc, delay = 1000) => {
    return React.lazy(async () => {
        await new Promise(resolve => setTimeout(resolve, delay));
        return importFunc();
    });
};

const OverViewAssignPage = LazyLoading(() => import('./Overview/OverViewAssignPage'));
const InProcessAssignPage = LazyLoading(() => import('./InProcess/InProcessAssignPage'));
const PreparePage = LazyLoading(() => import('./Prepare/PreparePage'));
const StatusPage = LazyLoading(() => import('../StatusComponent/StatusPage'));
const PrepareHwPage = LazyLoading(() => import('./Prepare/PrepareHwPage'));

function AssignmentPage() {
    return (
        <div className='w-full h-screen'>
            <Suspense fallback={<LoaderPage />}>
                <Routes>
                    <Route path='in-process' element={<InProcessAssignPage />}></Route>
                    <Route path='/' element={<OverViewAssignPage />}></Route>
                    <Route path='/prepare' element={<PreparePage />} />
                    <Route path='/prepare-homework' element={<PrepareHwPage />} />
                    <Route path='/*' element={<StatusPage status={404} />} />
                </Routes>
            </Suspense>

            <MenuContextPage/>
        </div>
    )
}

export default AssignmentPage